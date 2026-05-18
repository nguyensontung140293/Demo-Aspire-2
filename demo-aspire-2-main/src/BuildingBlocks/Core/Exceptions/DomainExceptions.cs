namespace BuildingBlocks.Core.Exceptions;

public class NotFoundException(string entity, object key)
    : Exception($"{entity} với id '{key}' không tồn tại.");

public class ValidationException(IEnumerable<string> errors)
    : Exception($"Validation thất bại: {string.Join("; ", errors)}")
{
    public IEnumerable<string> Errors { get; } = errors;
}

public class ConflictException(string message) : Exception(message);

public class ForbiddenException(string message = "Không có quyền thực hiện hành động này.")
    : Exception(message);
