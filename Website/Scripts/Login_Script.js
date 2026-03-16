let UserContainer = {
    UserID : 0,
    UserName : "",
    Role : ""
}

const body = document.body;
const header = document.querySelector("header");
const footer = document.querySelector("footer");
//const form = document.querySelectorAll("form");
const mode_switch = document.getElementById("mode_switch");

let showingLogin = false;
mode_switch.dark = true;
let register_process = false;
let update_process = 0;

const upd_ing = document.getElementById("updateuser");
const upd_legals = document.getElementById("UpdateLegalName");
const upd_first = document.getElementById("first_name");
const upd_last = document.getElementById("last_name");
const upd_contacts = document.getElementById("RegisterContacts");
const upd_phone = document.getElementById("phone");
const upd_email = document.getElementById("email");
const upd_warningbox = document.getElementById("upd_warningbox");

const sign_up = document.getElementById("sign_up");
const sign_button = document.getElementById("sign_up_button");
const sign_user = document.getElementById("sign_user");
const sign_pass = document.getElementById("sign_pass");
const sign_repass = document.getElementById("sign_repass");
const sign_warningbox = document.getElementById("sign_warningbox");

const log_in = document.getElementById("log_in");
const log_button = document.getElementById("log_in_button");
const log_user = document.getElementById("log_user");
const log_pass = document.getElementById("log_pass")
const log_warningbox = document.getElementById("log_warningbox");

const form_switch = document.querySelectorAll(".form_switch_button");

if (document.body.contains(document.getElementById("sign_up"))) 
{
    log_in.remove();
    form_switch.forEach(i => {
        i.addEventListener("click", switch_log_in);
    });
}

mode_switch.addEventListener("click", () => {
    mode_change();
});

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
    if (mode_switch.dark == true){
        document.querySelectorAll(".dark_nav").forEach((element) => {
            element.classList.remove("dark_nav");
            element.classList.add("light_nav");
        });

        document.querySelectorAll(".darkmode_animation").forEach((element) => {
            element.classList.remove("darkmode_animation");
            element.classList.add("lightmode_animation");
        });

        document.querySelectorAll(".dark").forEach((element) => {
            element.classList.remove("dark");
            element.classList.add("light");
        });

        mode_switch.dark = false;
    } else{
        document.querySelectorAll(".light_nav").forEach((element) => {
            element.classList.remove("light_nav");
            element.classList.add("dark_nav");
        });

        document.querySelectorAll(".lightmode_animation").forEach((element) => {
            element.classList.remove("lightmode_animation");
            element.classList.add("darkmode_animation");
        });

        document.querySelectorAll(".light").forEach((element) => {
            element.classList.remove("light");
            element.classList.add("dark");
        });

        mode_switch.dark = true;
    }

    /*
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
    */
}

async function UpdateLegalName(){
    if (upd_first.value == "" || upd_last.value == "") {
        upd_warningbox.innerText = "Please fill everything out...";
        upd_warningbox.classList.remove("hidden");
        return;
    }

    if (!upd_warningbox.classList.contains("hidden")){
        upd_warningbox.classList.add("hidden");
    }

    try{
        upd_legals.disabled = true;
        upd_legals.innerText = "Uploading name...";
        const response = await fetch("https://localhost:7142/api/User/update/name", { 
            method: "PUT", 
            headers: { "Content-Type": "application/json" }, 
            credentials: "include",
            body: JSON.stringify({
                id: UserContainer.userID,
                firstName: upd_first.value,
                lastName: upd_last.value
            })
        });

        if (!response.ok) { 
            upd_legals.disabled = false;
            upd_warningbox.classList.remove("hidden");
            upd_warningbox.innerText = `Please notify an admin! There was a problem. Error: ${response.status}`;

            console.log("POST with query failed");
        } else{
            upd_legals.disabled = true;
            upd_first.value = "";
            upd_last.value = "";
            update_process++;
            upd_legals.innerText = "Updated names successfully!";
            if (update_process >= 2){
                setTimeout(() =>{
                    window.location.href = "/";
                }, 5000);
            }
        }

    }catch (e){
        console.log("UPDATE LEGALS");
        console.log(e);
        upd_warningbox.classList.remove("hidden");
        upd_warningbox.innerText = `Please notify an admin! Error: ${e}`;
    }
}

async function UpdateContacts(){
    if (upd_email.value == "" || upd_phone.value == "") {
        upd_warningbox.innerText = "Please fill everything out...";
        upd_warningbox.classList.remove("hidden");
        return;
    }

    if (!upd_warningbox.classList.contains("hidden")){
        upd_warningbox.classList.add("hidden");
    }

    try{
        upd_contacts.disabled = true;
        upd_contacts.innerText = "Uploading contacts...";
        const response = await fetch("https://localhost:7142/api/User/update/contact", { 
            method: "PUT", 
            headers: { "Content-Type": "application/json" }, 
            credentials: "include",
            body: JSON.stringify({
                id: UserContainer.userID,
                phone: upd_phone.value,
                email: upd_email.value
            })
        });

        if (!response.ok) {
            upd_contacts.disabled = false;
            upd_warningbox.classList.remove("hidden");
            upd_warningbox.innerText = `Please notify an admin! There was a problem. Error: ${response.status}`;

            console.log("POST with query failed");
        } else{
            upd_contacts.disabled = true;
            upd_email.value = "";
            upd_phone.value = "";
            update_process++;
            upd_contacts.innerText = "Updated contacts successfully!";
            if (update_process >= 2){
                setTimeout(() =>{
                    window.location.href = "/";
                }, 5000);
            }
        }

    }catch (e){
        console.log("UPDATE LEGALS");
        console.log(e);
        upd_warningbox.classList.remove("hidden");
        upd_warningbox.innerText = `Please notify an admin! Error: ${e}`;
    }
}

async function Register(){
    if (sign_pass.value != sign_repass.value) {
        sign_warningbox.innerText = "The passwords do not match!";
        sign_warningbox.classList.remove("hidden");
        //alert("The passwords do not match!");
        return;
    }

    try{
        if (!await (await fetch(`https://localhost:7142/api/User/checkuser?username=${sign_user.value}`, { method: "GET" })).json()){
            sign_warningbox.innerText = "Username Already in use!";
            sign_warningbox.classList.remove("hidden");
            return;
        }

        if (!sign_warningbox.classList.contains("hidden")){
            sign_warningbox.classList.add("hidden");
        }

        sign_button.disabled = true;
        sign_button.innerText = "Signing up...";
        const response = await fetch(`https://localhost:7142/api/User/register?username=${sign_user.value}&password=${sign_pass.value}`, { method: "POST" });

        if (!response.ok) {
            sign_button.disabled = false;
            log_button.innerText = "Sign up";
            sign_warningbox.classList.remove("hidden");
            sign_warningbox.innerText = `Please notify an admin! There was a problem with your request. Error: ${response.status}`;

            console.log("POST with query failed");
        } else {
            //response.json()
            sign_button.disabled = true;
            sign_pass.value = "";
            sign_repass.value = "";
            sign_user.value = "";
            register_process = true;
            sign_button.innerText = "Account created! Please wait...";

            setTimeout(() => {
                switch_log_in();
            }, 5000);
        }

    }catch (e){
        console.log("SIGN UP");
        console.log(e);
        sign_warningbox.classList.remove("hidden");
        sign_warningbox.innerText = `Please notify an admin! Error: ${e}`;
    }
}

async function Login(){
    if (!log_warningbox.classList.contains("hidden")){
        log_warningbox.classList.add("hidden");
    }

    try{
        sign_button.disabled = true;
        sign_button.innerText = "Logging in...";
        const response = await fetch(`https://localhost:7142/api/User/login?username=${log_user.value}&password=${log_pass.value}`, { 
            method: "POST", 
            headers: { "Content-Type": "application/json" },
            credentials: "include" 
        });

        if (response.status === 401) {
            log_button.disabled = false;
            log_button.innerText = "Log in";
            log_warningbox.classList.remove("hidden");
            log_warningbox.innerText = "Your username or password is incorrect!";

            console.log("Incorrect Credentials!");
        } else if (response.ok) {
            log_button.disabled = true;
            log_pass.value = "";
            log_user.value = "";
            log_button.innerText = "Logged in successfully! Please wait!";
            setTimeout(() =>{
                if (register_process == true){
                    const data = response.json();
                    UserContainer.UserID = data.userID;
                    UserContainer.UserName = data.userName;
                    UserContainer.Role = data.role;
                    log_in.classList.add("hidden");
                    upd_ing.classList.remove("hidden");
                } else{
                    window.location.href = "/";
                }
            }, 5000);
        } else{
            log_button.disabled = false;
            log_warningbox.innerText = `Please notify an admin! There was a problem. Error: ${response.status}`;
        }

        /*const response = await fetch("https://api.example.com/users", {
            method: "POST",
            headers: {
              "Content-Type": "application/json"
            },
            body: JSON.stringify(userDto)
        });*/
    }catch (e){
        console.log("LOGIN");
        console.log(e);
        log_warningbox.classList.remove("hidden");
        log_warningbox.innerText = `Please notify an admin! Error: ${e}`;
    }
}

//sign_button.addEventListener("click", Register);
//log_button.addEventListener("click", Login);

upd_ing.addEventListener("submit", content => {
    content.preventDefault();
});

upd_legals.addEventListener("click", () => {
    UpdateLegalName();
});

upd_contacts.addEventListener("click", () => {
    UpdateContacts();
});

sign_up.addEventListener("submit", content => {
    content.preventDefault();
    Register();
});

log_in.addEventListener("submit", content => {
    content.preventDefault();
    Login();
});