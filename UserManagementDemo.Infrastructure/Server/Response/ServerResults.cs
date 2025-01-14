﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Infrastructure.Server
{
    public class ServerResponse<T> : ServerResponse, IServerResponse<T>
    {
        public T ApiResponse { get; set; }

        public new static ServerResponse<T> FailedMessage()
        {
            return new ServerResponse<T>
            {
                IsSuccessful = true
            };
        }

        public new static ServerResponse<T> FailedMessage(string message)
        {
            return new ServerResponse<T>
            {
                IsSuccessful = false,
                Message = message
            };
        }

        public new static ServerResponse<T> SuccessMessage()
        {
            return new ServerResponse<T>
            {
                IsSuccessful = true
            };
        }

        public new static ServerResponse<T> SuccessMessage(string message)
        {
            return new ServerResponse<T>
            {
                IsSuccessful = true,
                Message = message
            };
        }

        public static ServerResponse<T> SuccessMessage(T apiResponse)
        {
            return new ServerResponse<T>
            {
                IsSuccessful = true,
                ApiResponse = apiResponse
            };
        }

        public static ServerResponse<T> SuccessMessage(T apiResponse, string message)
        {
            return new ServerResponse<T>
            {
                IsSuccessful = true,
                ApiResponse = apiResponse,
                Message = message
            };
        }
    }
}