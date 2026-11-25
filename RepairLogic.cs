namespace Shop;
public static class RepairLogic
{
    public static TUser? Authenticate<TUser>(IEnumerable<TUser> users, string login)
        where TUser : class, IRegisteredUser
    {
        // D1 исправлено: убран запасной выбор "?? users.FirstOrDefault()".
        // Если логин не найден (или пустой), возвращается null -> вход отклоняется.
        return users.FirstOrDefault(u => u.Login == login.Trim());
    }

    public static void Back<TLine>(ICollection<TLine> draft)
    {
        // D2 исправлено: убрано draft.Clear().
        // Переход назад не отменяет заявку. Отмена — отдельная подтверждаемая команда.
    }

    public static List<TItem> Catalog<TItem>(IEnumerable<TItem> items, string query,
        string category, bool descending) where TItem : ICatalogItem
    {
        query = query.Trim();
        // D3 исправлено: категория И (&&) поиск; внутри поиска — имя ИЛИ описание.
        var filtered = items.Where(p =>
            (category == "Все категории" || p.Category == category) &&
            (p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
             p.Description.Contains(query, StringComparison.OrdinalIgnoreCase)));
        // D3 исправлено: сортировка по числу decimal (без ToString), при равной цене — по Id.
        return descending
            ? filtered.OrderByDescending(p => p.DiscountPrice).ThenBy(p => p.Id).ToList()
            : filtered.OrderBy(p => p.DiscountPrice).ThenBy(p => p.Id).ToList();
    }
}
