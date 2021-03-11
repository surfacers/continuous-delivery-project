FROM nginx:alpine
LABEL author="Bauer, Gattermayr, Hofer"
COPY ./dist/Hurace.Web /usr/share/nginx/html
EXPOSE 80 443
ENTRYPOINT [ "nginx", "-g", "daemon off;" ]