const card_template = document.getElementById("card-template");
const container = document.getElementById("company-container");
const check = document.getElementById("company-check");

const reg_word = document.getElementById("reg-word");
const reg_search = document.getElementById("reg-search");
const ref_btn = document.getElementById("ref-btn");

reg_search.addEventListener("click", GetSearch);
ref_btn.addEventListener("click", GetCompanies);
reg_word.addEventListener("keyup", Rerouted);

const page_tab = document.getElementById("page-tab");
const page_back = document.getElementById("page-back");
const page_counter = document.getElementById("page-counter");
const page_forward = document.getElementById("page-forward");

page_counter.index = 1;
page_counter.invoker = "load";

window.addEventListener("load", GetCompanies);

page_back.addEventListener("click", () =>{
    PageHandler(null, "previous");
});

page_forward.addEventListener("click", () =>{
    PageHandler(null, "next");
});

const QuickInvoker = {
    "load" : GetCompanies,
    "search" : GetSearch
}

function Rerouted() { // e
    reg_search.click();

    //incase of server overloading ^_^ - foxcode above
    /*if (e.key === "Enter"){
        reg_search.click();
    }*/
}

function PageHandler(event = null, action = "next", count = 24){
    switch(action){
        case "update" :
            page_back.disabled = false;
            page_forward.disabled = false;

            if(event != null){
                page_counter.innerText = page_counter.index;
            }

            if (page_counter.index <= 1) page_back.disabled = true;
            if (count < 24) page_forward.disabled = true;
            break;
        case "next" :
            page_counter.index++;
            QuickInvoker[page_counter.invoker](null, (page_counter.index * 24) - 24, 24);
            page_counter.innerText = page_counter.index;
            break;
        case "previous" :
            if (page_counter.index <= 1) return;
            page_counter.index--;
            QuickInvoker[page_counter.invoker](null, (page_counter.index * 24) - 24, 24);
            page_counter.innerText = page_counter.index;
            break;
        default :
            console.log("Nice try!");
            break;
    }
}

async function GetCompanies(event, skip = 0, take = 24) {
    try{
        const response = await fetch(`https://localhost:7142/api/Company/companies?skip=${skip}&take=${take}`);

        if (response.ok){
            const data = await response.json();

            page_tab.classList.remove("hidden");

            if (event != null) {
                page_counter.invoker = "load";
                page_counter.index = 1;
                PageHandler(event, "update");
            } else {
                PageHandler(null, "update", data.length);
            }

            if (data.length == 0){
                if (event != null) {
                    page_tab.classList.add("hidden");
                }

                container.innerHTML = "";
                check.innerText = "No Companies found!";
                return;
            }

            if (data.length < 24 && event != null){
                page_tab.classList.add("hidden");
            }

            check.innerText = "";
            container.innerHTML = "";

            data.forEach(element => {
                const template = card_template.content.cloneNode(true);
                const card = template.querySelector(".company-card");

                template.querySelector(".card-owner").textContent = element.ownerName;
                template.querySelector(".card-owner").href = `/User/${element.ownerID}`;
                template.querySelector(".card-company").textContent = element.companyName;
                template.querySelector(".card-description").textContent = element.description;
                template.querySelector(".card-button").href = `/Company/${element.id}`;

                card.style.opacity = 0;
                container.appendChild(template);
                setTimeout(() => {
                    card.style.opacity = 1;
                }, 1);
            });
        } else{
            if (event != null) {
                page_counter.classList.add("hidden");
            }

            container.innerHTML = "";
            check.innerText = "No Jobs found!";
        }
    } catch (e){
        console.log("LOAD JOBS");
        console.log(e);
    }
}

async function GetSearch(event, skip = 0, take = 24) {
    try{
        const response = await fetch(`https://localhost:7142/api/Company/companies/search?description=${reg_word.value}&skip=${skip}&take=${take}`);

        if (response.ok){
            const data = await response.json();

            page_tab.classList.remove("hidden");

            if (event != null) {
                page_counter.invoker = "search";
                page_counter.index = 1;
                PageHandler(event, "update");
            } else {
                PageHandler(null, "update", data.length);
            }

            if (data.length == 0){
                if (event != null) {
                    page_tab.classList.add("hidden");
                }

                container.innerHTML = "";
                check.innerText = "No Companies found!";
                return;
            }

            if (data.length < 24 && event != null){
                page_tab.classList.add("hidden");
            }

            check.innerText = "";
            container.innerHTML = "";

            data.forEach(element => {
                const template = card_template.content.cloneNode(true);
                const card = template.querySelector(".company-card");

                template.querySelector(".card-owner").textContent = element.ownerName;
                template.querySelector(".card-owner").href = `/User/${element.ownerID}`;
                template.querySelector(".card-company").textContent = element.companyName;
                template.querySelector(".card-description").textContent = element.description;
                template.querySelector(".card-button").href = `/Company/${element.id}`;
                
                card.style.opacity = 0;
                container.appendChild(template);
                setTimeout(() => {
                    card.style.opacity = 1;
                }, 1);
            });
        } else{
            if (event != null) {
                page_counter.classList.add("hidden");
            }

            container.innerHTML = "";
            check.innerText = "No Companies found!";
        }
    } catch (e){
        console.log("SEARCH COMPANIES");
        console.log(e);
    }
}
