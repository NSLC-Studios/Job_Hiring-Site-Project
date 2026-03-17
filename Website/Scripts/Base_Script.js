let UserContainer = {
    UserID : 0,
    UserName : "",
    Role : ""
}

const body = document.body;
const header = document.querySelector("header");
const footer = document.querySelector("footer");

const mode_switch = document.getElementById("mode_switch");
const register_tab = document.getElementById("register-tab");
const profile_tab = document.getElementById("profile-tab");
const profile = document.getElementById("profile");
const weather = document.getElementById("weather");
const weather_btn = document.getElementById("weather-btn");
const log_profile = document.getElementById("log-profile");
const admin_check = document.getElementById("admin-check");
const logout = document.getElementById("logout");

mode_switch.dark = true;

mode_switch.addEventListener("click", mode_change);

weather_btn.addEventListener("click", Weather);

logout.addEventListener("click", Logout);

window.addEventListener("load", () =>{
    UserSession();
    Weather();
});

function mode_change()
{
    if (mode_switch.dark == true){
        document.querySelectorAll(".dark_nav").forEach(element => {
            element.classList.remove("dark_nav");
            element.classList.add("light_nav");
        });

        document.querySelectorAll(".btn-outline-light").forEach(element => {
            element.classList.remove("btn-outline-light");
            element.classList.add("btn-outline-dark");
        });

        document.querySelectorAll(".darkmode_animation").forEach(element => {
            element.classList.remove("darkmode_animation");
            element.classList.add("lightmode_animation");
        });

        document.querySelectorAll(".dark_background").forEach(element => {
            element.classList.remove("dark_background");
            element.classList.add("light_background");
        });

        document.querySelectorAll(".dark").forEach(element => {
            element.classList.remove("dark");
            element.classList.add("light");
        });

        mode_switch.dark = false;
    } else{
        document.querySelectorAll(".light_nav").forEach(element => {
            element.classList.remove("light_nav");
            element.classList.add("dark_nav");
        });

        document.querySelectorAll(".btn-outline-dark").forEach(element => {
            element.classList.remove("btn-outline-dark");
            element.classList.add("btn-outline-light");
        });

        document.querySelectorAll(".lightmode_animation").forEach(element => {
            element.classList.remove("lightmode_animation");
            element.classList.add("darkmode_animation");
        });

        document.querySelectorAll(".light_background").forEach(element => {
            element.classList.remove("light_background");
            element.classList.add("dark_background");
        });

        document.querySelectorAll(".light").forEach(element => {
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

async function Weather(){
    try {
        weather_btn.disabled = true;
        const response = await fetch("https://localhost:7142/WeatherForecast");
        
        if (response.ok) {
            const data = await response.json();

            weather.innerText = `${data[0].summary} Weather.`;
        }

        weather_btn.disabled = false;
    } catch (e) {
        console.log("WEATHER");
        console.log(e);
    }
}

async function Logout() {
    try {
        const response = await fetch("https://localhost:7142/api/User/logout", { 
            method: "POST", 
            headers: { "Content-Type": "application/json" },
            credentials: "include"
        });
    
        if (response.ok) {
            location.reload();
        }
    } catch (e) {
        console.log("LOGOUT");
        console.log(e);
    }
}

async function UserSession(){
    try {
        const response = await fetch("https://localhost:7142/api/User/session", { 
            credentials: "include" 
        });
    
        if (response.ok) {
            const data = await response.json();
            UserContainer.UserID = data.id;
            UserContainer.UserName = data.userName;
            UserContainer.Role = data.role;

            register_tab.classList.add("hidden");
            register_tab.classList.remove("d-flex");
            profile.innerText = `Welcome ${UserContainer.UserName}!`;
            profile.href = `/Profile/${UserContainer.UserID}`;
            log_profile.href = `/Profile/${UserContainer.UserID}`;
            profile_tab.classList.remove("hidden");

            if (UserContainer.Role == "Admin"){
                admin_check.classList.remove("hidden");
            }
        }
    } catch (e) {
        console.log("USER SESSION");
        console.log(e);
    }
}
