$("#novoUsuario").on('click', function () {
    $("#modalUsuario").modal('show');
    $("#nomeUsuario").val("");
    $("#emailUsuario").val("");
});

$(".close").on('click', function () {
    $("#nomeUsuario").val("");
    $("#emailUsuario").val("");
    $("#editarNomeUsuario").val("");
    $("#editarEmailUsuario").val("");
});

function abrirEditarUsuario(id) {
    const url = `https://localhost:7129/User/${id}`;

    $.ajax({
        url: url,
        cache: true,
        type: 'GET',
        contentType: 'application/json',
        beforeSend: function () {
            $("#modalEditarUsuario").modal('show');
        },
        success: function (response) {
            $("#editarNomeUsuario").val(response.firstName);
            $("#editarEmailUsuario").val(response.email);
            $("#editarTelefoneUsuario").val(response.phoneNumber);
            $("#editarAtivoUsuario").prop('checked', response.active);
            $("#usuarioID").val(response.id);
        },
        error: function (xhr, status, error) {
            debugger
            $("#modalEditarUsuario").modal('hide');

            bootbox.alert({
                message: xhr.responseJSON.mensagem,
                closeButton: false
            });
        }
    });
}

function deletarUsuario(id) {
    const url = `https://localhost:7129/User/deletar/${id}`;

    $.ajax({
        url: url,
        cache: true,
        type: 'DELETE',
        contentType: 'application/json',
        beforeSend: function () {
            $("#main").addClass('d-none');
            $("#loading").removeClass('d-none');
        },
        success: function (response) {
            $("#loading").addClass('d-none');

            bootbox.alert({
                message: response.mensagem,
                closeButton: false,
                callback: function () {
                    $("#main").removeClass('d-none');
                    window.location.reload();
                }
            });
        },
        error: function (xhr, status, error) {
            $("#main").removeClass('d-none');
            $("#loading").addClass('d-none');

            bootbox.alert({
                message: xhr.responseJSON.mensagem,
                closeButton: false
            });
        }
    });
}

$("#editarUsuario").on('click', function () {
    const nome = $("#editarNomeUsuario").val();
    const email = $("#editarEmailUsuario").val();
    const telefone = $("#editarTelefoneUsuario").val();
    const ativo = $("#editarAtivoUsuario").prop('checked');
    const id = parseInt($("#usuarioID").val());

    if (!nome || !email) {
        alert("Nome e email são obrigatórios");
    } else {
        $.ajax({
            url: `https://localhost:7129/User/atualizar/${id}`,
            cache: true,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify({
                FirstName: nome,
                Email: email,
                PhoneNumber: telefone,
                Active: ativo
            }),
            beforeSend: function () {
                $("#modalEditarUsuario").modal('hide');
                $("#main").addClass('d-none');
                $("#loading").removeClass('d-none');
            },
            success: function (response) {
                $("#loading").addClass('d-none');

                bootbox.alert({
                    message: response.mensagem,
                    closeButton: false,
                    callback: function () {
                        $("#main").removeClass('d-none');
                        window.location.reload();
                    }
                });
            },
            error: function (xhr, status, error) {
                $("#main").removeClass('d-none');
                $("#loading").addClass('d-none');

                bootbox.alert({
                    message: xhr.responseJSON.mensagem,
                    closeButton: false
                });
            }
        });
    }
});

$("#salvarNovoUsuario").on('click', function () {
    const nome = $("#nomeUsuario").val();
    const email = $("#emailUsuario").val();

    if (!nome || !email) {
        alert("Nome e email são obrigatórios");
    } else {
        $.ajax({
            url: 'https://localhost:7129/User/novo',
            cache: true,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                FirstName: nome,
                Email: email
            }),
            beforeSend: function () {
                $("#modalUsuario").modal('hide');
                $("#main").addClass('d-none');
                $("#loading").removeClass('d-none');
            },
            success: function (response) {
                $("#loading").addClass('d-none');

                bootbox.alert({
                    message: response.mensagem,
                    closeButton: false,
                    callback: function () {
                        $("#main").removeClass('d-none');
                        window.location.reload();
                    }
                });
            },
            error: function (xhr, status, error) {
                $("#main").removeClass('d-none');
                $("#loading").addClass('d-none');

                bootbox.alert({
                    message: xhr.responseJSON.mensagem,
                    closeButton: false
                });
            }
        });
    }
});