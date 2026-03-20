const card_template = document.getElementById("card-template");
const container = document.getElementById("company-container");
const check = document.getElementById("company-check");

const reg_word = document.getElementById("reg-word");
const reg_search = document.getElementById("reg-search");

window.addEventListener("load", GetCompanies);

reg_search.addEventListener("click", GetSearch);

async function GetCompanies() {
    try{
        const response = await fetch("https://localhost:7142/api/Company/companies?skip=0&take=24");

        if (response.ok){
            const data = await response.json();

            if (data.length == 0){
                container.innerHTML = "";
                check.innerText = "No Companies found!";
                return;
            }

            check.innerText = "";
            container.innerHTML = "";

            data.forEach(element => {
                const template = card_template.content.cloneNode(true);

                template.querySelector(".card-owner").textContent = element.ownerName;
                template.querySelector(".card-owner").href = `/User/${element.ownerID}`;
                template.querySelector(".card-company").textContent = element.companyName;
                template.querySelector(".card-description").textContent = element.description;
                template.querySelector(".card-button").href = `/Company/${element.id}`;

                container.appendChild(template);
            });
        } else{
            container.innerHTML = "";
            check.innerText = "No Jobs found!";
        }
    } catch (e){
        console.log("LOAD JOBS");
        console.log(e);
    }
}

async function GetSearch() {
    try{
        const response = await fetch(`https://localhost:7142/api/Company/companies/search?description=${reg_word.value}&skip=0&take=24`);

        if (response.ok){
            const data = await response.json();

            if (data.length == 0){
                container.innerHTML = "";
                check.innerText = "No Companies found!";
                return;
            }

            check.innerText = "";
            container.innerHTML = "";

            data.forEach(element => {
                const template = card_template.content.cloneNode(true);

                template.querySelector(".card-owner").textContent = element.ownerName;
                template.querySelector(".card-owner").href = `/User/${element.ownerID}`;
                template.querySelector(".card-company").textContent = element.companyName;
                template.querySelector(".card-description").textContent = element.description;
                template.querySelector(".card-button").href = `/Company/${element.id}`;
                
                container.appendChild(template);
            });
        } else{
            container.innerHTML = "";
            check.innerText = "No Companies found!";
        }
    } catch (e){
        console.log("SEARCH COMPANIES");
        console.log(e);
    }
}
