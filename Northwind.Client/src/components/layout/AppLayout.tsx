import Navbar from "./Navbar";
import Container from "./Container";

export default function AppLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <Navbar />
      <main className="flex-1 py-8">
        <Container>{children}</Container>
      </main>
    </div>
  );
}
