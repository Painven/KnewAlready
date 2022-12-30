using AutoMapper;
using KnewAlreadyCore;
using KnewAlreadyWebApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Timers;

namespace KnewAlreadyWebApp.Pages;

public partial class LastRequestsPage
{
    [Inject] public SuggetWebApiSwaggerClient apiClient { get; set; }
    [Inject] public IMapper mapper { get; set; }

    List<SuggestActionModel> userRequestItems = new List<SuggestActionModel>();
    List<SuggestActionModel> newItems = new List<SuggestActionModel>();
    List<string> availableCategories = new List<string>();
    System.Timers.Timer timer;

    [Parameter] public string Username { get; set; }

    public string getItemStatusIconClass(SuggestActionModel item)
    {
        if (item.IsCompleted)
        {
            return "fa-hands-helping completed";
        }
        else if (item.IsExpired)
        {
            return "fa-times";
        }
        else
        {
            return "fa-question";
        }

    }

    protected override async Task OnInitializedAsync()
    {
        timer = new System.Timers.Timer((int)TimeSpan.FromSeconds(3).TotalMilliseconds);
        timer.Elapsed += TimerTick;
        timer.Start();

        await LoadData();
    }

    protected override async Task OnParametersSetAsync()
    {
        timer.Stop();
        userRequestItems.Clear();
        await LoadData();
        timer.Start();
    }

    private async void TimerTick(object sender, ElapsedEventArgs e)
    {
        List<SuggestActionModel> tempItems = userRequestItems;

        await LoadData();

        newItems.Clear();
        foreach (var item in userRequestItems)
        {
            var alreadyAddedItem = tempItems.FirstOrDefault(i => i.Guid == item.Guid);
            if (alreadyAddedItem == null || alreadyAddedItem.IsCompleted != item.IsCompleted)
            {
                newItems.Add(item);
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadData()
    {
        var data = await apiClient.SuggestActionsAllAsync(Username);

        userRequestItems = mapper.Map<IEnumerable<SuggestActionModel>>(data).ToList();

        availableCategories = data.GroupBy(d => d.CategoryName).Select(g => g.Key).ToList();
    }

    public void Dispose()
    {
        if (timer != null)
        {
            timer.Elapsed -= TimerTick;
            timer.Dispose();
        }
    }
}
