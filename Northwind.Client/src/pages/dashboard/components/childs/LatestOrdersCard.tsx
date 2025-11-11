import { Card, CardContent } from "../../../../components/ui/Card";

type Props = {
  settings: { title: string };
  data: { recentOrders: { orderId: number; customer: string; date: string; total: number }[] };
};
export default function LatestOrdersCard({ settings, data }: Props) {
  return (
    <Card>
        <CardContent className="p-6">
          <h2 className="text-lg font-semibold mb-4">{settings.title}</h2>
          <div className="overflow-x-auto max-h-[400px] overflow-y-auto border rounded">
            <table className="w-full text-left text-sm">
              <thead className="bg-gray-100 sticky top-0">
                <tr>
                  <th className="p-2 border-b">Order #</th>
                  <th className="p-2 border-b">Kund</th>
                  <th className="p-2 border-b">Datum</th>
                  <th className="p-2 border-b">Totalt</th>
                </tr>
              </thead>
              <tbody>
                {data!.recentOrders.length ? (
                  data!.recentOrders.map((o) => (
                    <tr key={o.orderId} className="hover:bg-blue-50">
                      <td className="p-2 border-b">{o.orderId}</td>
                      <td className="p-2 border-b">{o.customer}</td>
                      <td className="p-2 border-b">{new Date(o.date).toLocaleDateString()}</td>
                      <td className="p-2 border-b">{o.total.toLocaleString()} kr</td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan={4} className="text-center p-4 text-gray-500 italic">
                      Inga ordrar hittades.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </CardContent>
      </Card>
  )
  
}