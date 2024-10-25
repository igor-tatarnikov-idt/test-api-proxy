local content_transformer = require("content_transformer")
local tokenizer_client = require("tokenizer_client")

local function pre_process_request_body(transformer)
    ngx.req.read_body()
    local body_data = ngx.req.get_body_data()

    if body_data == nil then
        return
    end

    ngx.log(ngx.DEBUG, "Original request body: ", body_data)
    
    local fields_to_transform_values_for = {
        "recipientBankAccountNumber",
        "RecipientBankAccountNumber",
        "senderName", 
        "SenderName",
        "recipientName",
        "RecipientName"
    };

    local modified_body_data = content_transformer.transform_json(body_data, fields_to_transform_values_for, transformer)

    ngx.req.set_body_data(modified_body_data)

    ngx.log(ngx.DEBUG, "Modified request body: ", modified_body_data)
end

local function tokenize()
    pre_process_request_body(tokenizer_client.tokenize)
end

local function detokenize()
    pre_process_request_body(tokenizer_client.detokenize)
end

return {
    tokenize = tokenize,
    detokenize = detokenize
}