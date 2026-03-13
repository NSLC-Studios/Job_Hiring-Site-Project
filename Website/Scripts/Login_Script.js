
const body = document.body;
const header = document.querySelector("header");
const footer = document.querySelector("footer");
const form = document.querySelectorAll("form");

let showingLogin = false;

const sign_up = document.getElementById("sign_up");
const sign_button = document.getElementById("sign_up_button");
const sign_user = document.getElementById("sign_user");
const sign_pass = document.getElementById("sign_pass");
const sign_repass = document.getElementById("sign_repass");

const log_in = document.getElementById("log_in");
const log_button = document.getElementById("log_in_button");
const log_user = document.getElementById("log_user");
const log_pass = document.getElementById("log_pass")

const form_switch = document.querySelectorAll(".form_switch_button");

if (document.body.contains(document.getElementById("sign_up"))) 
{
    log_in.remove();
    form_switch.forEach(i => {
        i.addEventListener("click", switch_log_in);
    });
}

const mode_switch = document.getElementById("mode_swich")

mode_switch.addEventListener("click",function(){
    mode_change();
})

function switch_log_in() {

    if (showingLogin === false) {
        sign_up.remove();
        body.appendChild(log_in);
        showingLogin = true;
    } else {
        log_in.remove();
        body.appendChild(sign_up);
        showingLogin = false;
    }
}

function mode_change()
{
    if(header.classList.contains("dark_nav"))
    {
        header.classList.remove("dark_nav");
        footer.classList.remove("dark_nav");
        body.classList.remove("darkmode_animation");
        form.forEach(f => f.classList.remove("dark"))
        header.classList.add("light_nav");
        footer.classList.add("light_nav");
        body.classList.add("lightmode_animation");
        form.forEach(f => f.classList.add("light"))
    }
    else {
        header.classList.remove("light_nav");
        footer.classList.remove("light_nav");
        body.classList.remove("lightmode_animation");
        form.forEach(f => f.classList.remove("light"));
    
        header.classList.add("dark_nav");
        footer.classList.add("dark_nav");
        body.classList.add("darkmode_animation");
        form.forEach(f => f.classList.add("dark"));
    }
}

async function register(){
    try{
        const response = await fetch("https://localhost:5094/api/User/register?username=12223&password=1234")// `http://localhost/api/User/register?username=${sign_user.value}&password=${sign_pass.value}`, { method: "POST" });

        if (!response.ok) {
            throw new Error("POST with query failed");
        } else {
            console.log(response.ok);
        }

        /*const response = await fetch("https://api.example.com/users", {
            method: "POST",
            headers: {
              "Content-Type": "application/json"
            },
            body: JSON.stringify(userDto)
        });*/
    }catch (e){
        console.log(e);
    }
}

sign_button.addEventListener("click", register);