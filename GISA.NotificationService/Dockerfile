FROM rrvieira/pythondapr:1.0

COPY ["GISA.NotificationService/requirements.txt", "./requirements.txt"]
COPY ["GISA.NotificationService/server.py", "./server.py"]

RUN pip install -r requirements.txt

COPY ./dapr /dapr