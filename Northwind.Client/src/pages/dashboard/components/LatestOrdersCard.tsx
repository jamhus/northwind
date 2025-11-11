import { useState } from "react";
import { Card, CardContent } from "../../../components/ui/Card";
import Table from "../../../components/common/Table";
import OrderDetailsModal from "../../orders/modals/OrderDetailsModal";
import type { Order } from "../../../api/order.services";

type Props = {
  settings: { title: string };
  data: {
    orderId: number;
    customer: string;
    employee?: string;
    date: string;
    total: number;
  }[];
};

export default function LatestOrdersCard({ settings, data }: Props) {
  const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);

  return (
   <>
     {data?.length ? (
          <Table
            title={settings.title}
            data={data.map((d) => ({
              orderId: d.orderId,
              orderDate: d.date,
              customerName: d.customer,
              employeeName: d.employee ?? "–",
              total: d.total,
            }))}
            columns={[
              { key: "orderId", label: "ID", width: "60px" },
              {
                key: "orderDate",
                label: "Datum",
                render: (o) =>
                  new Date(o.orderDate).toLocaleDateString("sv-SE"),
              },
              { key: "customerName", label: "Kund" },
              { key: "employeeName", label: "Anställd" },
              {
                key: "total",
                label: "Totalt",
                render: (o) =>
                  o.total.toLocaleString("sv-SE", {
                    minimumFractionDigits: 1,
                    maximumFractionDigits: 1,
                  }) + " kr",
              },
            ]}
            onRowClick={(o) =>
              setSelectedOrder({
                orderId: o.orderId,
                orderDate: o.orderDate,
                customerName: o.customerName,
                employeeName: o.employeeName,
                total: o.total,
              } as Order)
            }
          />
        ) : (
          <div className="text-gray-500 italic text-center py-4">
            Inga ordrar hittades.
          </div>
        )}

        {/* 🔹 Modal */}
        {selectedOrder && (
          <OrderDetailsModal
            orderId={selectedOrder.orderId}
            isOpen={!!selectedOrder}
            onClose={() => setSelectedOrder(null)}
          />
        )}
   </>
  );
}
