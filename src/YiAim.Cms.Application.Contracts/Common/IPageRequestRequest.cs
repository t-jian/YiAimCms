using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YiAim.Cms;
public interface IPageRequestRequest
{
    int Limit { get; set; }
    int Page { get; set; }
}
public interface ISortedRequest
{
    string Sorting { get; set; }
}
public interface IWhereRequest
{
    string Condition { get; set; }
}

public class PagingInput : IPageRequestRequest
{

    [Range(1, int.MaxValue)]
    public virtual int Page { get; set; } = 1;
    [Range(1, int.MaxValue)]
    public virtual int Limit { get; set; } = 10;
    public PagingInput()
    {

    }
    public PagingInput(int page = 1, int limit = 10)
    {
        this.Page = page;
        this.Limit = limit;
    }
}

public class PagingWhereInput : PagingInput, IWhereRequest
{
    public virtual string Condition { get; set; }
}

public class PageSortedRequestDto : PagingInput, ISortedRequest
{
    public virtual string Sorting { get; set; }
}

public class PagedList<T> where T : class
{
    public long Count { get; set; }

    public List<T> Items { get; set; }
}