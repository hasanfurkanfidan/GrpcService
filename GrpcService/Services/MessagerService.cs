using Google.Protobuf;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcMessageService
{
    public class MessagerService : Message.MessageBase
    {
        public override Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MessageResponse
            {
                Message = $"{request.Name} adlı kullanıcı size {request.Message} mesajını gönderdi."
            });
        }
        public override async Task SendMessageStream(MessageRequest request, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(200);
                await responseStream.WriteAsync(new MessageResponse
                {
                    Message = $"{request.Name} adlı kullanıcı size {request.Message} mesajını gönderdi."
                });

            }
        }
        public  override async Task<MessageResponse> SendMessageStreamFromClient(IAsyncStreamReader<MessageRequest> requestStream, ServerCallContext context)
        {
            var response = new MessageResponse();
            while(await requestStream.MoveNext())
            {
                response.Message = $"{requestStream.Current.Name } adlı kullanıcı size {requestStream.Current.Message} mesajını gönderdi";
            }
            return response;
        }
        public override async Task SendMessageStreamBiDirectioanal(IAsyncStreamReader<MessageRequest> requestStream, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        {
            while(await requestStream.MoveNext())
            {
                for (int i = 0; i < 10; i++)
                {
                    await responseStream.WriteAsync(new MessageResponse
                    {
                        Message = $"{requestStream.Current.Name } adlı kullanıcı size {requestStream.Current.Message} mesajını gönderdi"
                    });
                }
               

            }
        }
    }
}
