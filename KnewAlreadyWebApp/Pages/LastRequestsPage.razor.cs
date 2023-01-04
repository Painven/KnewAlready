using AutoMapper;
using KnewAlreadyCore;
using KnewAlreadyWebApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Timers;

namespace KnewAlreadyWebApp.Pages;

public partial class LastRequestsPage
{
    const int MAXIMUM_ITEMS_IN_PAGE = 30;

    [Inject] public SuggestApiSwaggerClient apiClient { get; set; }
    [Inject] public IMapper mapper { get; set; }
    [CascadingParameter] public AppUser loginerUser { get; set; }

    private List<SuggestActionModel> userRequestItems = new List<SuggestActionModel>();
    private IEnumerable<SuggestActionModel> filteredUserRequestItems
    {
        get
        {
            IEnumerable<SuggestActionModel> filteredSource = userRequestItems;

            if (new string[] { selectedFilterStatus, selectedFilterCategory }.Any(f => f != "Все"))
            {
                if (selectedFilterStatus != "Все")
                {
                    if (selectedFilterStatus == "Только законченные")
                    {
                        filteredSource = filteredSource.Where(i => i.IsConfirmed);
                    }
                    if (selectedFilterStatus == "Только ожидающие")
                    {
                        filteredSource = filteredSource.Where(i => !i.IsConfirmed && !i.IsExpired);
                    }
                }

                if (selectedFilterCategory != "Все")
                {
                    filteredSource = filteredSource.Where(i => i.CategoryName == selectedFilterCategory);
                }
            }

            return filteredSource.Take(MAXIMUM_ITEMS_IN_PAGE);
        }
    }
    private List<SuggestActionModel> newItems = new List<SuggestActionModel>();
    private List<string> availableCategories = new List<string>();
    private System.Timers.Timer timer = new System.Timers.Timer((int)TimeSpan.FromSeconds(10).TotalMilliseconds);

    [Parameter] public Guid Id { get; set; }

    public string ContextUsername
    {
        get => (Id != Guid.Empty ? Id : loginerUser.Id).ToString();
    }

    string selectedFilterStatus = "Все";
    string selectedFilterCategory = "Все";

    public string getItemClasses(SuggestActionModel item)
    {
        string str = "suggest-action-item";

        if (newItems.Contains(item))
        {
            str += " new-item";
        }

        if (item.IsExpired && !item.IsConfirmed)
        {
            str += " expired";
        }

        return str;
    }
    public string getItemStatusIconClass(SuggestActionModel item)
    {
        if (item.IsConfirmed)
        {
            return "fa-hands-helping completed";
        }
        else if (item.IsExpired)
        {
            return "fa-times";
        }

        return "fa-question blink";
    }

    protected override async Task OnInitializedAsync()
    {
        timer.Elapsed += TimerTick;
        timer.Start();
    }

    protected override async Task OnParametersSetAsync()
    {
        await LoadData();
    }

    private async void TimerTick(object sender, ElapsedEventArgs e)
    {
        List<SuggestActionModel> tempItems = userRequestItems;

        await LoadData();

        newItems.Clear();
        foreach (var item in userRequestItems)
        {
            var alreadyAddedItem = tempItems.FirstOrDefault(i => i.Guid == item.Guid);
            if (alreadyAddedItem == null || alreadyAddedItem.IsConfirmed != item.IsConfirmed)
            {
                newItems.Add(item);
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadData()
    {
        userRequestItems?.Clear();

        var data = await apiClient.SuggestActionsAllAsync(ContextUsername);

        userRequestItems = mapper.Map<IEnumerable<SuggestActionModel>>(data).ToList();

        availableCategories = data.GroupBy(d => d.CategoryName).Select(g => g.Key).ToList();
    }

    public void Dispose()
    {
        if (timer != null)
        {
            timer.Elapsed -= TimerTick;
            timer?.Dispose();
        }
    }
}
