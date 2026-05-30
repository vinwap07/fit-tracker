namespace Application.Exceptions;

public class NotFoundException(string name, object key) : Exception($"Сущность \"{name}\" ({key}) не найдена.");