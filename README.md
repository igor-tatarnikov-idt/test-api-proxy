# test-api-proxy

![Provider Proxy sandbox-Sandbox Stg1 drawio](https://github.com/user-attachments/assets/669ec770-fa21-466c-b564-d9c13732ed25)

# Proxy

### Input
- `X-Target-Host` - where to proxy request to. I.e. `http://rg:8080`
- `X-Tokenization-Mode` - one of predefined values expected - `tokenize`, `detokenize`. Missing header is treated as `as-is` and skips request payload manipulation
- Request field names list - to process the transformation on. I.e. `senderName`, `recipientName`, `recipienBankAccountNumber`
- Tokenizer API config
