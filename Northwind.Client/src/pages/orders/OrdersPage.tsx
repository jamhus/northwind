import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import Table from "../../components/common/Table";
import Loader from "../../components/common/Loader";
import OrderDetailsModal from "./modals/OrderDetailsModal";
import Pagination from "../../components/common/Pagination";
import { orderService, type Order } from "../../api/order.services";

export default function OrdersPage() {
  const [page, setPage] = useState(1);
  const [selected, setSelected] = useState<Order | null>(null);
  const pageSize = 10;

  const { data, isLoading, isError } = useQuery({
    queryKey: ["orders", page],
    queryFn: () => orderService.getAll(page, pageSize),
  });

  if (isLoading) return <Loader />;
  if (isError) return <div>Det gick inte att hämta ordrar.</div>;

  const orders = data?.items || [];

  return (
    <div className="p-8">
      <Table<Order>
        title="Ordrar"
        data={orders}
        columns={[
          { key: "orderId", label: "ID" },
          { key: "orderDate", label: "Datum", render: (o) => new Date(o.orderDate ?? "").toLocaleDateString() },
          { key: "customerName", label: "Kund" },
          { key: "employeeName", label: "Anställd" },
          { key: "shipCountry", label: "Land" },
          { key: "total", label: "Totalt", render: (o) => o.total.toFixed(2) + " kr" },
        ]}
        onEdit={(o) => setSelected(o)}
      />
      <Pagination
        page={data?.page ?? 1}
        totalPages={data?.totalPages ?? 1}
        onPageChange={setPage}
      />

      {selected && (
        <OrderDetailsModal
          orderId={selected.orderId}
          isOpen={!!selected}
          onClose={() => setSelected(null)}
        />
      )}
    </div>
  );
}
