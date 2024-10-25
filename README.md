# test-api-proxy

![Provider Proxy sandbox-Sandbox Stg1 drawio](https://github.com/user-attachments/assets/669ec770-fa21-466c-b564-d9c13732ed25)

# Proxy

### Input
- `X-Target-Host` - where to proxy request to. I.e. `http://rg:8080`
- `X-Tokenization-Mode` - one of predefined values expected - `tokenize`, `detokenize`. Missing header is treated as `as-is` and skips request payload manipulation
- Request field names list - to process the transformation on. I.e. `senderName`, `recipientName`, `recipienBankAccountNumber`
- Tokenizer API config



### References
- OpenResty https://openresty.org/en/
- Docs
  - https://nginx.org/en/docs/
  - https://www.lua.org/manual/5.3/contents.html#contents
  - https://openresty-reference.readthedocs.io/en/latest/
- GitHub
  - https://github.com/openresty/lua-nginx-module
  - https://github.com/ledgetech/lua-resty-http
- Misc
  - Karl Seguin introduction to OpenResty [pt1](https://www.openmymind.net/An-Introduction-To-OpenResty-Nginx-Lua/) [pt2](https://www.openmymind.net/An-Introduction-To-OpenResty-Part-2/) [pt3](https://www.openmymind.net/An-Introduction-To-OpenResty-Part-3/)
  - Beatifying Nginx logs - [link](https://betterstack.com/community/guides/logging/how-to-view-and-configure-nginx-access-and-error-logs/#step-4-structuring-nginx-access-logs)