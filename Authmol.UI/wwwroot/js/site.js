const viaCEPBaseUrl = "https://viacep.com.br/ws";
const UrlSuffix = "json";
const cepInput = document.getElementById("Input_CEP");
const cepErrorSpan = document.getElementById("cep-error-span");
const logradouroInput = document.getElementById("Input_Logradouro")
const complementoInput = document.getElementById("Input_Complemento")
const cidadeInput = document.getElementById("Input_Cidade")
const bairroInput = document.getElementById("Input_Bairro")
const estadoInput = document.getElementById("Input_Estado")
const cepSpinner = document.getElementById("cep-spinner");
const submitSpinner = document.getElementById("submit-spinner");
const registerForm = document.getElementById("registerForm");
const submitBtn = document.getElementById("registerSubmit");
const btnText = document.getElementById("btn-text");
const loginForm = document.getElementById("login-form");
const loginBtn = document.getElementById("login-btn");
const loginBtnText = document.getElementById("login-btn-text");
const loginSpinner = document.getElementById("login-spinner");

if (cepInput) {
    cepInput.addEventListener("keyup", async e => {
        const valid = validar(e.target.value);
        cepErrorSpan.innerHTML = "";
        submitBtn.disabled = false;
        if (!valid) {
            cepErrorSpan.innerHTML = "CEP inválido";
            submitBtn.disabled = true;
            limparCampos();
            return;
        }

        const numeros = e.target.value.replace(/\D/g, '');
        await getEndereco(numeros);
    });
}

if (registerForm) {
    registerForm.addEventListener("submit", () => {
        if($(loginForm).valid()){
            submitBtn.disabled = true;
            btnText.innerHTML = "";
            submitSpinner.classList.remove("d-none");
        }
    });
}

if (loginForm) {
    loginForm.addEventListener("submit", () => {
        loginBtn.disabled = true;
        loginBtnText.innerHTML = "";
        loginSpinner.classList.remove("d-none");
    });
}

const validar = val => {
    const regex = /^.*\d{5}-?\d{3}$/
    const formato = regex.test(val);
    const tamanhoCorreto = val.replace(/\D/g, '').length === 8;

    return formato && tamanhoCorreto;
}

const getEndereco = async cep => {
    cepSpinner.classList.remove("d-none");
    const response = await fetch(`${viaCEPBaseUrl}/${cep}/${UrlSuffix}`);
    const result = await response.text();
    let resultParsed = JSON.parse(result);

    if (resultParsed.erro) {
        cepErrorSpan.innerHTML = "CEP não encontrado";
        limparCampos();
        cepSpinner.classList.add("d-none");
        return;
    }

    logradouroInput.value = resultParsed.logradouro;
    complementoInput.value = resultParsed.complemento;
    cidadeInput.value = resultParsed.localidade;
    bairroInput.value = resultParsed.bairro;
    estadoInput.value = resultParsed.uf;
    cepSpinner.classList.add("d-none");
}

const limparCampos = () => {
    logradouroInput.value = "";
    complementoInput.value = "";
    cidadeInput.value = "";
    bairroInput.value = "";
    estadoInput.value = "";
}
