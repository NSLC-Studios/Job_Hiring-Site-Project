
const body = document.body;
const header = document.querySelector("header");
const footer = document.querySelector("footer");
const form = document.querySelectorAll("form")


const sign_up = document.getElementById("sign_up");
const log_in = document.getElementById("log_in")

log_in.remove();
let showingLogin = false;
const form_switch = document.querySelectorAll(".form_switch_button");

const mode_switch = document.getElementById("mode_swich")

mode_switch.addEventListener("click",function(){
    mode_change();
})

form_switch.forEach(i => {
    i.addEventListener("click", switch_log_in);
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