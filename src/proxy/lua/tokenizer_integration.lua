local http = require "resty.http"
local cjson = require "cjson"

local tokenize_api = "http://tokenizer:8080/tokenize"
local detokenize_api = "http://tokenizer:8080/detokenize"
local valuePattern = "(\\d|\\s|\\+|\\-|\\=|\\'|\\w)+"

function tokenize_request(data)
    local modified_body = data

    local pii_fields = { "senderName", "recipientName", "SenderName", "RecipientName" }

    for _,field in ipairs(pii_fields) do
        modified_body =  tokenize(modified_body, field)
    end

    local pci_fields = { "recipientBankAccountNumber", "RecipientBankAccountNumber" }

    for _,field in ipairs(pci_fields) do
        modified_body =  tokenize(modified_body, field)
    end

    return modified_body
end

function detokenize_request(data)
    local modified_body = data

    local pii_fields = { "senderName", "recipientName", "SenderName", "RecipientName" }

    for _,field in ipairs(pii_fields) do
        modified_body =  detokenize(modified_body, field)
    end

    local pci_fields = { "recipientBankAccountNumber", "RecipientBankAccountNumber" }

    for _,field in ipairs(pci_fields) do
        modified_body =  detokenize(modified_body, field)
    end

    return modified_body
end

function tokenize(data, field_name)
    local modified_data = ngx.re.gsub(
            data,
            '"' .. field_name .. '"\\s*\\:\\s*"' .. valuePattern .. '"',
            function(match)
                -- i.e. "RecipientBankAccountNumber":"Vr_nhVSCFU0gv3"
                local matched_item = match[0]

                local json_string = "{" .. matched_item .. "}"
                local table = cjson.decode(json_string)
                local value = table[field_name]

                local token, err = call_tokenize_api(value)

                if not token then
                    ngx.log(ngx.ERR, "Failed to process value: ", matched_item, " Error: ", err)
                    return matched_item
                end

                return '"' .. field_name .. '":"' .. token .. '"'
            end,
            "jo"
    )

    return modified_data
end

function detokenize(data, field_name)
    local modified_data = ngx.re.gsub(
            data,
            '"' .. field_name .. '"\\s*\\:\\s*"' .. valuePattern .. '"',
            function(match)
                -- i.e. "RecipientBankAccountNumber":"Vr_nhVSCFU0gv3"
                local matched_item = match[0]

                local json_string = "{" .. matched_item .. "}"
                local table = cjson.decode(json_string)
                local value = table[field_name]

                local raw, err = call_detokenize_api(value)

                if not raw then
                    ngx.log(ngx.ERR, "Failed to process value: ", matched_item, " Error: ", err)
                    return matched_item
                end

                return '"' .. field_name .. '":"' .. raw .. '"'
            end,
            "jo"
    )

    return modified_data
end

function call_tokenize_api(raw_value)
    return call_api(tokenize_api, raw_value)
end

function call_detokenize_api(raw_value)
    return call_api(detokenize_api, raw_value)
end

-- Function to tokenize or detokenize a value via HTTP call
function call_api(api_url, payload)
    local httpc = http.new()

    -- Make the API call
    local res, err = httpc:request_uri(api_url, {
        method = "POST",
        body = cjson.encode(payload),
        headers = {
            ["Content-Type"] = "application/json"
        }
    })

    if not res then
        ngx.log(ngx.ERR, "API call failed: ", err)
        return nil, err
    end

    local api_response = res.body

    if res.status == 200 then
        return api_response, nil
    else
        return nil, "Error from API: " .. res.body
    end
end

return {
    tokenize_request = tokenize_request,
    detokenize_request = detokenize_request
}