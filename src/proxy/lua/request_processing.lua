local _M = {}

local valuePattern = "(\\d|\\s|\\+|\\-|\\=|\\'|\\w)+";

function _M.detokenize_request(data)
    local modifiedData =  _M.detokenize_pci(data)
    modifiedData =  _M.detokenize_pii(modifiedData)

    return modifiedData
end

function _M.tokenize_request(data)
    local modifiedData =  data

    local pii_fields = { "senderName", "recipientName", "SenderName", "RecipientName" }

    for _,field in ipairs(pii_fields) do
        modifiedData =  _M.tokenize_pii(modifiedData, field)
    end

    local pci_fields = { "recipientBankAccountNumber", "RecipientBankAccountNumber" }

    for _,field in ipairs(pci_fields) do
        modifiedData =  _M.tokenize_pci(modifiedData, field)
    end

    return modifiedData
end

function _M.detokenize_pci(data)
    local wrappedValuePattern = "pci\\(\\{" .. valuePattern .. "\\}\\)"

    return _M.detokenize(data, wrappedValuePattern)
end

function _M.tokenize_pci(data, fieldName)
    return _M.tokenize(data, fieldName, "pci")
end

function _M.detokenize_pii(data)
    -- Ex: "pii({John Wick})" ==> "John Wick"
    local wrappedValuePattern = "pii\\(\\{" .. valuePattern .. "\\}\\)"

    return _M.detokenize(data, wrappedValuePattern)
end

function _M.tokenize_pii(data, fieldName)
    return _M.tokenize(data, fieldName, "pii")
end

function _M.detokenize(data, wrappedValuePattern)
    local modified_data = ngx.re.gsub(data, wrappedValuePattern, function(match)
        -- https://openresty-reference.readthedocs.io/en/latest/Lua_Nginx_API/#ngxregmatch
        local wrappedItem = match[0]
        local matchingItemsIterator = ngx.re.gmatch(wrappedItem, valuePattern)
        local unwrappedItem, err =matchingItemsIterator()

        unwrappedItem, err = matchingItemsIterator()

        return unwrappedItem[0]
    end)

    return modified_data
end

function _M.tokenize(data, fieldName, wrapper)
    -- Ex: "key":"value" ==> "key":"pci({value})"
    local modified_data = ngx.re.gsub(
        data,
        '"' .. fieldName .. '"\\s*\\:\\s*"' .. valuePattern .. '"',
        function(match)
            -- i.e. "RecipientBankAccountNumber":"Vr_nhVSCFU0gv3"
            local matchedItem = match[0]
            local modified_string = string.gsub(
                matchedItem,
                '"' .. fieldName .. '"%s*:%s*"([^"]+)"',
                '"' .. fieldName ..'":"' .. wrapper .. '({%1})"')

            return modified_string
        end,
        "jo"
    )

    return modified_data
end

return _M