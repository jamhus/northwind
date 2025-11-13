import Table from "../../../components/common/Table";

type SupplierSales = {
  supplier: string;
  totalSales: number;
};

type Props = {
  settings: { title: string };
  data: SupplierSales[] | SupplierSales;
};

export default function SupplierTotalSalesCard({ settings, data }: Props) {
  const items = Array.isArray(data) ? data : [data];

  return (
    <Table<SupplierSales>
      title={settings.title}
      data={items}
      maxHeight={450}
      columns={[
        {
          key: "supplier",
          label: "Leverantör",
        },
        {
          key: "totalSales",
          label: "Total försäljning",
          render: (s) => s.totalSales.toLocaleString() + "$",
        },
      ]}
    ></Table>
  );
}
