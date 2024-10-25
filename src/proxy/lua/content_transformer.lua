local cjson = require "cjson"

local valuePattern = "(\\d|\\s|\\+|\\-|\\=|\\'|\\w)+"

-- looks for JSON formatted key-value pairs and replaces the value
-- i.e. "Name": "Alex" ---> "Name":"A***"
local function transform_json_field(content, field_name, transformer)
    local modified_data = ngx.re.gsub(
        content,
        '"' .. field_name .. '"\\s*\\:\\s*"' .. valuePattern .. '"',
        function(match)
            local matched_item = match[0]
            local json_string = "{" .. matched_item .. "}"
            local table = cjson.decode(json_string)
            local value = table[field_name]
            local transformed_value = transformer(value)

            return '"' .. field_name .. '":"' .. transformed_value .. '"'
        end,
        "jo"
    )

    return modified_data
end

local function transform_json(content, fields, transformer)
    local modified_content = content

    for _, field in ipairs(fields) do
        modified_content =  transform_json_field(modified_content, field, transformer)
    end

    return modified_content
end

return {
    transform_json = transform_json
}