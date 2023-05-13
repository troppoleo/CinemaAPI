using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// https://codewithmukesh.com/blog/pagination-in-aspnet-core-webapi/
namespace CinemaBL.Paging
{
    //internal class GenericPaging
    //{
    //}


    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            //Succeeded = true;
            //Message = string.Empty;
            //Errors = null;
            Data = data;
        }
        public T Data { get; set; }
        //public bool Succeeded { get; set; }
        //public string[] Errors { get; set; }
        //public string Message { get; set; }
    }

    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //public Uri FirstPage { get; set; }
        //public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        //public Uri NextPage { get; set; }
        //public Uri PreviousPage { get; set; }
        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            //this.Message = null;
            //this.Succeeded = true;
            //this.Errors = null;
        }
    }


    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? FilmName { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize, string filmName = null)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            FilmName = filmName.ToLower();
        }
    }
}
