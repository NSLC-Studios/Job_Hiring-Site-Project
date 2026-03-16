let UserContainer = {
    UserID : 0,
    UserName : "",
    Role : ""
}

UserSession();

const body = document.body;
const header = document.querySelector("header");
const footer = document.querySelector("footer");
const mode_switch = document.getElementById("mode_switch");

const register_tab = document.getElementById("register-tab");
const profile_tab = document.getElementById("profile-tab");
const profile = document.getElementById("profile");

mode_switch.dark = true;

mode_switch.addEventListener("click", () => {
    mode_change();
});

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

        document.querySelectorAll(".dark_background").forEach((element) => {
            element.classList.remove("dark_background");
            element.classList.add("light_background");
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

        document.querySelectorAll(".light_background").forEach((element) => {
            element.classList.remove("light_background");
            element.classList.add("dark_background");
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
            profile_tab.classList.remove("hidden");
        }
    } catch (e) {
        console.log(e);
    }
}
