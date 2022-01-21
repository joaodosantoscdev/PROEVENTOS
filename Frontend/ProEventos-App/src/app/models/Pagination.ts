export class Pagination {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  itemsPerPage: number;
}

export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
