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
    ".ico" : "image/x-icon",
    ".js"  : "application/javascript",
    ".svg" : "image/svg+xml",
};

function ReplyError(sender, error = "Not Specified"){
    sender.writeHead(500, {"Content-Type": "text/html"}); // 521
    sender.write(`There was a problem with requesting the content, this is an internal server problem. Please remain calm and try again later.\n\nIn the event that the problem persists longer than 15 minutes, please click here. > <a href="127.0.0.1:60/${Help_Requested_Domain}" target="_blank">Request aid<a/>`);
    sender.write(`<br>The thrown error was: ${error}`);
    sender.end();
}

function SendEmpty(sender){
    sender.writeHead(404, {"Content-Type": "text/html"}); // 530
            
    fs.readFile(`../Website/Empty.html`, (err, payload) =>{
        if(err){
            ReplyError(sender, err.message);
            return;
        }

        sender.end(payload);
    });
    return;
}

function SendResponse(sender, target, type){
    fs.readFile(`../Website/${target}${type}`, (err, payload) =>{
        if(err){
            //throw err;
            //console.log(err)
            
            if (err.code === "ENOENT"){
                SendEmpty(sender);
                return;
            } else{
                ReplyError(sender, err.message);
                return;
            }
        }

        // const payload = data;
        sender.writeHead(200, {"Content-Type": ContentSuffixTable[type]}); // "text/html"
        //res.write();
        sender.end(payload);
    });
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
            // 404 purposes deprecated
            target = "Contdact";
            type = ".html";
            break;
        case "login" :
            target = "Login";
            type = ".html";
            break;
        case "companies" :
            target = "Companies";
            type = ".html";
            break;
        case "company" :
            target = "Company";
            type = ".html";
            break;
        case "job" :
            target = "Job";
            type = ".html";
            break;
        case "about" :
            target = "About";
            type = ".html";
            break;
        case "tos" :
            target = "Tos";
            type = ".html";
            break;
        case "faq" :
            target = "Faq";
            type = ".html";
            break;
        case "styles" :
            target = `Styles/${query[2].replace(".css", "")}`
            type = ".css";
            break;
        case "scripts" :
            target = `Scripts/${query[2].replace(".js", "")}`
            type = ".js";
            break;
        case "images" :
            switch(query[2]){
                case "vectors" :
                    target = `Images/Vectors/${query[3].replace(".svg", "")}`
                    type = ".svg";
                    break;
                case "icons" :
                    target = `Images/Icons/${query[3].replace(".ico", "")}`
                    type = ".ico";
                    break;
                default :
                    switch(query[2].split(".")[1]){
                        case "jpg" :
                            target = `Images/${query[2].replace(".jpg", "")}`
                            type = ".jpg";
                            break;
                        case "png" :
                            target = `Images/${query[2].replace(".png", "")}`
                            type = ".png";
                            break;
                    }
                    break;
            }
            break;
        case "favicon.ico" :
            target = "logo";
            type = ".ico";
            break;
        default :
            // throw connection to stop interference with other web utilities
            try {
                SendEmpty(res);
            return;
            } catch (err) {
                ReplyError(res, err.message);
            }
    }
    try {
        // throw new error;
        SendResponse(res, target, type);
    } catch (err) {
        console.log(err);
        ReplyError(res, JSON.stringify(err));
        
        //res.writeHead(500, {"Content-Type": "text/html"}); // 521
        //res.write(`There was a problem with requesting the content, this is an internal server problem. Please remain calm and try again later.\n\nIn the event that the problem persists longer than 15 minutes, please click here. > <a href="127.0.0.1:60/${Help_Requested_Domain}" target="_blank">Request aid<a/>`)
        //res.end();
        return;
    }
}).listen(80);
