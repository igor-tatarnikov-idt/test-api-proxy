# Use OpenResty base image (Nginx with Lua support)
FROM openresty/openresty:alpine-fat

# Copy the custom Nginx configuration file into the Docker image
RUN ls
COPY ["src/proxy/nginx.conf", "/usr/local/openresty/nginx/conf/nginx.conf"]
COPY ["src/proxy/lua/.", "/usr/local/openresty/nginx/conf/lua/."]

# Install Lua dependencies (resty-http for making HTTP requests)
RUN apk update 
RUN apk add --no-cache luarocks
RUN /usr/local/openresty/luajit/bin/luarocks install lua-resty-http 

RUN mkdir var/log/nginx
RUN cd /var/log/nginx/
RUN touch access.log
RUN touch error.log
RUN chmod 750 *.log


# Expose port 80 for incoming traffic
EXPOSE 80

# Start OpenResty (Nginx) when the container starts
CMD ["/usr/local/openresty/bin/openresty", "-g", "daemon off;"]
