local api_client = require("api_client")

local tokenizer_base_url = "http://tokenizer:8080"

local function tokenize(raw_value)
    local token, _ = api_client.post(tokenizer_base_url .. "/tokenize", raw_value)

    if token == nil then
        return raw_value
    end

    return token
end

local function detokenize(original_value)
    local raw_value, _ = api_client.post(tokenizer_base_url .. "/detokenize", original_value)

    if raw_value == nil then
        return original_value
    end

    return raw_value
end

return {
    tokenize = tokenize,
    detokenize = detokenize
}