function apagarRegistro(id, controller) {
    if (confirm('Confirma a exclusão do registro?'))
        location.href = controller + '/Delete?id=' + id;
}

function SalvaNomePortifolio(url) {
    var idPortifolio = $("#IdPortifolio").val();
    var nomePortifolio = $("#NomePortifolioModal").val();
    var idUsuario = $("#IdUsuario").val();

    var data =
    {
        Model: {
            Id: idPortifolio,
            Nome: nomePortifolio,
            IdUsuario: idUsuario
        },
        Operacao: "A"
    };

     $.ajax({
         url: url,
         type: "POST",
         data: data,
         success: function (data) {
             location.href = "/Portifolio/Index";
         },
         error: function () {
             alert("Erro ao carregar o conteúdo.");
         }
     });
};

function SalvaNomeCarteira(url) {
    var idCarteira = $("#IdCarteira").val();
    var nomeCarteira = $("#NomeCarteiraModal").val();
    var idPortifolio = $("#IdPortifolio").val();

    var data =
    {
        Model: {
            Id: idCarteira,
            Descricao: nomeCarteira,
            Id_Portifolio: idPortifolio
        },
        Operacao: "A"
    };

    $.ajax({
        url: url,
        type: "POST",
        data: data,
        success: function (data) {
            location.href = "/Carteira/CarregaCarteira/" + idCarteira;
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });
};


function ConsultaCarteira(url, id) {
    window.location.href = url + "/" + id;
}