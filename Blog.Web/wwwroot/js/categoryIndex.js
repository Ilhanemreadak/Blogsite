$(document).ready(function () {
    // DataTables için özel tarih sıralama fonksiyonu ekleyelim
    jQuery.extend(jQuery.fn.dataTable.ext.type.order, {
        "turkish-date-pre": function (d) {
            // Tarih formatını "dd.mm.yyyy" olarak kabul ediyoruz
            if (d === null || d === "") {
                return 0;
            }

            var parts = d.split('.');
            // Eğer tarih formatı "dd.mm.yyyy" ise parse edelim
            var day = parseInt(parts[0], 10);
            var month = parseInt(parts[1], 10);
            var year = parseInt(parts[2], 10);

            // Yılı, ayı ve günü geri döndürelim (Date nesnesine çevirebiliriz)
            return new Date(year, month - 1, day).getTime();
        }
    });

    // DataTables tanımlaması
    $('#categoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [],
        language: {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "0": "",
                    "1": "1 kayıt seçildi"
                }
            }
        },
        columnDefs: [
            {
                targets: [2],
                type: 'turkish-date'
            }
        ]
    });
});
