local _M = {}

local valuePattern = "(\\d|\\s|\\+|\\-|\\=|\\'|\\w)+";

function _M.detokenize(data)
    local modifiedData =  _M.detokenize_pci(data)
    modifiedData =  _M.detokenize_pii(modifiedData)

    return modifiedData
end

function _M.detokenize_pci(data)
    local wrappedValuePattern = "pci\\(\\{" .. valuePattern .. "\\}\\)"

    return _M.substitute_token(data, wrappedValuePattern, valuePattern)
end

function _M.detokenize_pii(data)
    local wrappedValuePattern = "pii\\(\\{" .. valuePattern .. "\\}\\)"

    return _M.substitute_token(data, wrappedValuePattern, valuePattern)
end

function _M.substitute_token(data, wrappedValuePattern, valuePattern)
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

return _M