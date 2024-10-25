local http = require "resty.http"
local cjson = require "cjson"

local function send(url, method, payload)
    local http_client = http.new()

    ngx.log(ngx.DEBUG, "calling ", url)

    local res, err = http_client:request_uri(url, {
        method = method,
        body = cjson.encode(payload),
        headers = {
            ["Content-Type"] = "application/json"
        }
    })

    if res == nil then
        ngx.log(ngx.ERR, "failed: ", err)
        return nil, err
    end

    local api_response = res.body

    if res.status == 200 then
        return api_response, nil
    else
        ngx.log(ngx.WARN, "received status code: ", res.status, " at ", url)
        return nil, api_response
    end
end

local function post(url, payload)
    return send(url, "POST", payload)
end

return {
    post = post,
    send = send
}