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

const form_switch = document.getElementById("form-switch");
form_switch.login = false

form_switch.addEventListener("click", Switch_Forms);

function Switch_Forms() {
    if (form_switch.login == false) {
        sign_up.classList.add("hidden");
        log_in.classList.remove("hidden");
        form_switch.innerText = "Sign Up!";
        form_switch.login = true;
    } else {
        sign_up.classList.remove("hidden");
        log_in.classList.add("hidden");
        form_switch.innerText = "Log In!";
        form_switch.login = false;
    }
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
                id: UserContainer.UserID,
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
                id: UserContainer.UserID,
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
                }, 1000);
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
    if (sign_user.value == "" || sign_repass.value == "") {
        sign_warningbox.innerText = "Please fill everything out!";
        sign_warningbox.classList.remove("hidden");
        return;
    }

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
            sign_button.innerText = "Sign up";
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
                Switch_Forms();
            }, 1000);
        }

    }catch (e){
        console.log("SIGN UP");
        console.log(e);
        sign_button.innerText = "Sign up";
        sign_warningbox.classList.remove("hidden");
        sign_warningbox.innerText = `Please notify an admin! Error: ${e}`;
    }
}

async function Login(){
    if (log_user.value == "" || log_pass.value == "") {
        log_warningbox.innerText = "Please fill everything out!";
        log_warningbox.classList.remove("hidden");
        return;
    }
    
    if (!log_warningbox.classList.contains("hidden")){
        log_warningbox.classList.add("hidden");
    }

    try{
        log_button.disabled = true;
        log_button.innerText = "Logging in...";
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
            }, 1000);
        } else{
            log_button.disabled = false;
            log_button.innerText = "Log in";
            log_warningbox.innerText = `Please notify an admin! There was a problem. Error: ${response.status}`;
        }
    }catch (e){
        console.log("LOGIN");
        console.log(e);
        log_button.innerText = "Log in";
        log_warningbox.classList.remove("hidden");
        log_warningbox.innerText = `Please notify an admin! Error: ${e}`;
    }
}

//sign_button.addEventListener("click", Register);
//log_button.addEventListener("click", Login);

window.addEventListener("load", async () => {
    const path = window.location.pathname.toLowerCase();

    await UserSession();

    if (UserContainer.UserID != 0) {
        log_button.disabled = true;
        sign_button.disabled = true;

        log_button.innerText = "You are already logged in!"
        sign_button.innerText = "You are already logged in!"
    }

    if (path.endsWith("in/login")) {
        form_switch.innerText = "Sign Up!";
        form_switch.login = true;
        sign_up.classList.add("hidden");
        log_in.classList.remove("hidden");
    }
});

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
