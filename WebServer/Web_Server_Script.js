const http = require("http");
const fs = require("fs");

const Help_Requested_Domain = "Service_Down";

http.createServer((req, res) => {
    console.log("hi");
    const query = req.url.toLowerCase().split("?")[0].split("/");
    let target;
    switch(query[0][0]){
        case "" :
            target = "Home";
            break;
        case "contact" :
            target = "Contact";
            break;
        default :
            // throw connection to stop interference with other web utilities
            return;
    }
    try {
        const payload = fs.readFile(`../Website/${target}.html`)
        res.writeHead(200, {"Content-Type": "text/html"});
        res.write(payload);
        res.end();
    } catch {
        res.writeHead(500, {"Content-Type": "text/html"}); // 521
        res.write(`There was a problem with requesting the content, this is an internal server problem. Please remain calm and try again later.\n\nIn the event that the problem persists longer than 15 minutes, please click here. > <a href="127.0.0.1:60/${Help_Requested_Domain}" target="_blank">Request aid<a/>`)
        res.end();
    }
}).listen(80);