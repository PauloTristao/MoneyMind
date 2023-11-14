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

    var model =
    {
        model: {
            Id: idCarteira,
            Descricao: nomeCarteira,
            Id_Portifolio: idPortifolio
        },
        Operacao: "A"
    };


    $.ajax({
        url: '../../Carteira/ConsultaCarteiraNome',
        type: "POST",
        data: model,
        async: false,
        success: function (data) {
            if (data == true) {
                alert("Nome já utilizado!");
            }
            else {
                $.ajax({
                    url: url,
                    type: "POST",
                    data: model,
                    success: function (data) {
                        location.href = "/Carteira/CarregaCarteira/" + idCarteira;
                    },
                    error: function () {
                        alert("Erro ao carregar o conteúdo.");
                    }
                });
            }
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });


};

function CriaNovaCarteira(url) {
    var nomeCarteira = $("#nomeNovaCarteira").val();
    var idPortifolio = $("#IdPortifolio").val();

    var model =
    {
        model: {
            Descricao: nomeCarteira,
            Id_Portifolio: idPortifolio
        },
        Operacao: "I"
    }

    $.ajax({
        url: 'Carteira/ConsultaCarteiraNome',
        type: "POST",
        data: model,
        async: false,
        success: function (data) {
            if (data == true) {
                alert("Nome já utilizado!");
            }
            else {
                $.ajax({
                    url: url,
                    type: "POST",
                    data: model,
                    success: function (data) {
                        location.href = "/Portifolio/Index";
                    },
                    error: function () {
                        alert("Erro ao carregar o conteúdo.");
                    }
                });
            }
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });



}

function ConsultaCarteira(url, id) {
    window.location.href = url + "/" + id;
}

function salvaMovimentacao() {
    var vAtivo = document.getElementById('ativo').value;
    var vCarteira = $('#carteira').val();
    var vOperacao = document.getElementById('operacao').value;
    var vQuantidade = document.getElementById('Quantidade').value;
    var vPreco = document.getElementById('Preco').value;
    var vDataMovimentacao = document.getElementById('DataMovimentacao').value;

    if (!vAtivo || !vCarteira || !vOperacao || !vQuantidade || !vPreco || !vDataMovimentacao) {
        alert("Por favor, preencha todos os campos obrigatórios.");
        return;
    }

    var model = {
        Id_carteira: vCarteira,
        Id_Ativo: vAtivo,
        Id_Operacao: vOperacao,
        Quantidade: vQuantidade,
        Preco: vPreco,
        DataMovimentacao: vDataMovimentacao
    };

    // Inclua a propriedade "Operacao" diretamente no objeto model
    model.Operacao = "I";

    $.ajax({
        url: "/Movimentacao/Save",
        Type: "POST",
        data: model,
        success: function () {
            location.href = "../../Carteira/CarregaCarteira/" + vCarteira;
        }
    });
    location.href = "../../Carteira/CarregaCarteira/" + vCarteira;
}