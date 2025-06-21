function confirmDelete(id) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "Esta acción no se puede deshacer.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = '/Orders/Delete/' + id;
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    const feedback = document.getElementById("feedback-container");
    if (!feedback) return;

    const successMsg = feedback.dataset.success;
    const errorMsg = feedback.dataset.error;

    if (successMsg) {
        Swal.fire({
            icon: 'success',
            title: 'Éxito',
            text: successMsg,
            confirmButtonText: 'OK'
        });
    }

    if (errorMsg) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: errorMsg,
            confirmButtonText: 'OK'
        });
    }
});

function confirmRestore(id) {
    Swal.fire({
        title: '¿Restaurar orden?',
        text: "Esta acción recuperará la orden eliminada.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#198754',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, restaurar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            const form = document.getElementById(`restore-form-${id}`);
            if (form) {
                form.submit();
            }
        }
    });
}