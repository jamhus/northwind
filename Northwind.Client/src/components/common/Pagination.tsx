import { ArrowLeft, ArrowRight } from "lucide-react";

type Props = {
  page: number;
  totalPages: number;
  onPageChange: (page: number) => void;
};

export default function Pagination({ page, totalPages, onPageChange }: Props) {
  
  const getPageNumbers = () => {
    const pages: (number | string)[] = [];
    const maxVisible = 5;
    const half = Math.floor(maxVisible / 2);

    if (totalPages <= maxVisible) {
      for (let i = 1; i <= totalPages; i++) pages.push(i);
    } else if (page <= half + 1) {
      for (let i = 1; i <= maxVisible; i++) pages.push(i);
      pages.push("…", totalPages);
    } else if (page >= totalPages - half) {
      pages.push(1, "…");
      for (let i = totalPages - (maxVisible - 1); i <= totalPages; i++)
        pages.push(i);
    } else {
      pages.push(1, "…");
      for (let i = page - half + 1; i <= page + half - 1; i++) pages.push(i);
      pages.push("…", totalPages);
    }

    return pages;
  };

  return (
    <div className="flex justify-center items-center gap-2 mt-6">
      {/* Föregående */}
      <button
        onClick={() => onPageChange(page - 1)}
        disabled={page <= 1}
        className="px-3 py-1 rounded bg-gray-200 disabled:opacity-50 hover:bg-gray-300"
      >
        <ArrowLeft />
      </button>

      {/* Sidnummer */}
      {getPageNumbers().map((p, idx) =>
        typeof p === "number" ? (
          <button
            key={idx}
            onClick={() => onPageChange(p)}
            className={`px-3 py-1 rounded ${
              p === page
                ? "bg-blue-600 text-white font-semibold"
                : "bg-gray-100 hover:bg-gray-200"
            }`}
          >
            {p}
          </button>
        ) : (
          <span key={idx} className="px-2 text-gray-500">
            {p}
          </span>
        )
      )}

      {/* Nästa */}
      <button
        onClick={() => onPageChange(page + 1)}
        disabled={page >= totalPages}
        className="px-3 py-1 rounded bg-gray-200 disabled:opacity-50 hover:bg-gray-300"
      >
       <ArrowRight />
      </button>
    </div>
  );
}
