function apagarRegistro(id, controller) {
    if (confirm('Confirma a exclusão do registro?'))
        location.href = controller + '/Delete?id=' + id;
}

function SalvaNomePortifolio(url) {
    debugger;
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
             $.ajax({
                 url: "/Portifolio/Index"
             });
         },
         error: function () {
             alert("Erro ao carregar o conteúdo.");
         }
     });
};
