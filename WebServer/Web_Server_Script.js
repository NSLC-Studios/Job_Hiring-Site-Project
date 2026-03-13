const http = require("http");
const fs = require("fs");
const { error } = require("console");
const { json } = require("stream/consumers");

const Help_Requested_Domain = "Service_Down";

const ContentSuffixTable = {
    ".html" : "text/html",
    ".css" : "text/css",
    ".png" : "image/png",
    ".jpg" : "image/jpeg",
    ".ico" : "image/x-icon"
};

function ReplyError(sender, error = "Not Specified"){
    sender.writeHead(500, {"Content-Type": "text/html"}); // 521
    sender.write(`There was a problem with requesting the content, this is an internal server problem. Please remain calm and try again later.\n\nIn the event that the problem persists longer than 15 minutes, please click here. > <a href="127.0.0.1:60/${Help_Requested_Domain}" target="_blank">Request aid<a/>`);
    sender.write(`<br>The thrown error was: ${error}`);
    sender.end();
}

http.createServer((req, res) => {
    console.log("hi");
    const query = req.url.toLowerCase().split("?")[0].split("/");
    console.log(req.url);
    let target;
    let type = ".html";
    switch(query[1]){
        case "" :
            target = "Home";
            type = ".html";
            break;
        case "contact" :
            target = "Contact";
            type = ".html";
            break;
        case "condtact" :
            target = "Contdact";
            type = ".html";
            break;
        case "styles" :
            target = `Styles`
            type = ".css";
            break;
        case "favicon.ico" :
            target = "logo";
            type = ".ico";
            break;
        default :
            // throw connection to stop interference with other web utilities
            res.writeHead(530, {"Content-Type": "text/html"});
            res.end();
            return;
            //return;
    }
    try {
        // throw new error;
        fs.readFile(`../Website/${target}${type}`, (err, payload) =>{
            if(err){
                //throw err;
                //console.log(err)

                ReplyError(res, err.message);
                return;
            }

            // const payload = data;
            res.writeHead(200, {"Content-Type": ContentSuffixTable[type]}); // "text/html"
            //res.write();
            res.end(payload);
        })
    } catch (err) {
        console.log(err);
        ReplyError(res, JSON.stringify(err));
        
        //res.writeHead(500, {"Content-Type": "text/html"}); // 521
        //res.write(`There was a problem with requesting the content, this is an internal server problem. Please remain calm and try again later.\n\nIn the event that the problem persists longer than 15 minutes, please click here. > <a href="127.0.0.1:60/${Help_Requested_Domain}" target="_blank">Request aid<a/>`)
        //res.end();
        return;
    }
}).listen(80);