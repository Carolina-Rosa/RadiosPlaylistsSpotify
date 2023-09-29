import React, { useMemo } from "react";
import { useTable, usePagination } from "react-table";
import "./styles.scss";

export default function TableLogs({ logs, radio }) {
    // https://www.youtube.com/watch?v=A9oUTEP-Q84

    const data = useMemo(() => logs ?? [], [logs]);
    const columns = useMemo(
        () => [
            {
                Header: "Timestamp",
                accessor: "TimeStamp"
            },
            {
                Header: "Description",
                accessor: "Message"
            }
        ],
        []
    );

    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        page,
        nextPage,
        previousPage,
        canNextPage,
        canPreviousPage,
        prepareRow,
        pageOptions,
        state
    } = useTable({ columns, data }, usePagination);

    const { pageIndex } = state;

    return (
        <div className="table-logs">
            <table {...getTableProps()}>
                <thead>
                    {headerGroups.map((headerGroup) => (
                        <tr {...headerGroup.getHeaderGroupProps()}>
                            {headerGroup.headers.map((column) => (
                                <th {...column.getHeaderProps()}>
                                    {column.render("Header")}
                                </th>
                            ))}
                        </tr>
                    ))}
                </thead>
                <tbody {...getTableBodyProps}>
                    {page.map((row) => {
                        prepareRow(row);
                        return (
                            <tr {...row.getRowProps()}>
                                {row.cells.map((cell) => (
                                    <td {...cell.getCellProps()}>
                                        {cell.render("Cell")}
                                    </td>
                                ))}
                            </tr>
                        );
                    })}
                </tbody>
            </table>
            <div className="change-page-section">
                <span>
                    Page{" "}
                    <strong>
                        {pageIndex + 1} of {pageOptions.length}
                    </strong>{" "}
                </span>
                <button
                    className="button-change-page"
                    onClick={() => previousPage()}
                    disabled={!canPreviousPage}
                >
                    Previous
                </button>
                <button
                    className="button-change-page"
                    onClick={() => nextPage()}
                    disabled={!canNextPage}
                >
                    Next
                </button>
            </div>
        </div>
    );
}
