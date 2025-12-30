using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SchoolProcurement.Domain.Entities;

namespace SchoolProcurement.Api.Helper
{
    public static class PurchaseOrderPdfGenerator
    {
        public static byte[] Generate(PurchaseOrder po)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    page.Header().Text($"Purchase Order - {po.PONumber}")
                        .FontSize(18).Bold().AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().PaddingTop(25).Text($"Branch : {po.Branch?.Name ?? ""}");
                        col.Item().Text($"Order Date: {po.OrderDate:dd-MMM-yyyy}");
                        col.Item().Text($"Expected Delivery Date: {po.ExpectedDeliveryDate:dd-MMM-yyyy}");
                        col.Item().PaddingBottom(10).Text($"Supplier: {po.SupplierContact?.Name}");
                        col.Item().LineHorizontal(1);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn();
                                c.ConstantColumn(60);
                                c.ConstantColumn(60);
                                c.ConstantColumn(80);
                            });

                            table.Header(h =>
                            {
                                h.Cell().Text("Product").Bold();
                                h.Cell().Text("Qty").Bold();
                                h.Cell().Text("Price").Bold();
                                h.Cell().Text("Total").Bold();
                            });

                            foreach (var item in po.Items)
                            {
                                table.Cell().Text(item.Product!.Name);
                                table.Cell().Text(item.OrderedQty.ToString());
                                table.Cell().Text(item.UnitPrice.ToString("0.00"));
                                table.Cell().Text((item.UnitPrice * item.OrderedQty).ToString("0.00"));
                            }
                        });

                        col.Item().LineHorizontal(1);
                        col.Item().AlignRight().Text($"Total Amount: {po.TotalAmount:0.00}")
                            .FontSize(14).Bold();
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("School Procurement System")
                        .FontSize(10);
                });
            }).GeneratePdf();
        }
    }
}
