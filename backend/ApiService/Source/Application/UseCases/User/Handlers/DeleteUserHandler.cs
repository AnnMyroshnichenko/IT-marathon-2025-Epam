using CSharpFunctionalExtensions;
using Epam.ItMarathon.ApiService.Application.UseCases.User.Commands;
using Epam.ItMarathon.ApiService.Domain.Abstract;
using Epam.ItMarathon.ApiService.Domain.Shared.ValidationErrors;
using FluentValidation.Results;
using MediatR;
using RoomAggregate = Epam.ItMarathon.ApiService.Domain.Aggregate.Room.Room;

namespace Epam.ItMarathon.ApiService.Application.UseCases.User.Handlers
{
    public class DeleteUserHandler(IRoomRepository roomRepository)
        : IRequestHandler<DeleteUserCommand, Result<RoomAggregate, ValidationResult>>
    {
        ///<inheritdoc/>
        public async Task<Result<RoomAggregate, ValidationResult>> Handle(DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            var roomResult = await roomRepository.GetByUserCodeAsync(request.UserCode, cancellationToken);
            if(roomResult.IsFailure)
            {
                return roomResult;
            }

            var room = roomResult.Value;
            room.DeleteUser(request.UserId);

            var updatedResult = await roomRepository.UpdateAsync(room, cancellationToken);
            if (updatedResult.IsFailure)
            {
                return Result.Failure<RoomAggregate, ValidationResult>(new BadRequestError([
                    new ValidationFailure(string.Empty, updatedResult.Error)
                    ]));
            }

            var updatedRoomResult = await roomRepository.GetByUserCodeAsync(request.UserCode, cancellationToken);
            return updatedRoomResult;
        }
    }
}