import { useQuery } from "@tanstack/react-query";
import Loader from "../../../components/common/Loader";
import Table from "../../../components/common/Table";
import { orderService } from "../../../api/order.services";
import ModalWrapper from "../../products/modals/ModalWrapper";

type Props = {
  orderId: number;
  isOpen: boolean;
  onClose: () => void;
};

export default function OrderDetailsModal({ orderId, isOpen, onClose }: Props) {
  const { data, isLoading } = useQuery({
    queryKey: ["order", orderId],
    queryFn: () => orderService.getById(orderId),
    enabled: isOpen,
  });

  if (isLoading) return <Loader />;
  if (!data) return null;

  const totalSum = data.details.reduce(
    (sum, d) => sum + d.unitPrice * d.quantity * (1 - d.discount),
    0
  );

  return (
    <ModalWrapper
      isOpen={isOpen}
      onClose={onClose}
      width="w-2xl"
      title={`Order #${data.orderId} – ${data.customerName}`}
    >
      <div className="flex flex-col gap-2 text-sm mb-4">
        <p>
          <strong>Datum:</strong>{" "}
          {new Date(data.orderDate ?? "").toLocaleDateString()}
        </p>
        <p>
          <strong>Anställd:</strong> {data.employeeName}
        </p>
        <p>
          <strong>Land:</strong> {data.shipCountry}
        </p>
      </div>

      <Table
        title="Orderdetaljer"
        data={data.details}
        maxHeight={450}
        columns={[
          { key: "productName", label: "Produkt" },
          { key: "supplierName", label: "Leverantör" },
          {
            key: "unitPrice",
            label: "Pris",
            render: (d) => `${d.unitPrice.toFixed(2)} $`,
            width: "100px",
          },
          { key: "quantity", label: "Antal", width: "80px" },
          {
            key: "discount",
            label: "Rabatt",
            render: (d) => `${(d.discount * 100).toFixed(0)}%`,
            width: "80px",
          },
        ]}
      />

      {/* Total i footer */}
      <div className="mt-4 flex justify-end text-sm font-medium text-gray-700">
        <span>Totalt:</span>
        <span className="ml-2 font-semibold">{totalSum.toFixed(2)} $</span>
      </div>
    </ModalWrapper>
  );
}
