package main

import (
    "encoding/json"
    "log"
    "os"
    "github.com/sendgrid/sendgrid-go"
    "github.com/sendgrid/sendgrid-go/helpers/mail"
    "github.com/streadway/amqp"
    "github.com/joho/godotenv"
)

type Message struct {
    Email   string `json:"email"`
    Subject string `json:"subject"`
    Body    string `json:"body"`    
}

func main() {
    if err := godotenv.Load(); err != nil {
        log.Fatalf("Erro ao carregar o arquivo .env: %s", err)
    }

    rabbitMQURL := os.Getenv("RABBITMQ_URL")
    queueName := os.Getenv("QUEUE_NAME")
    sendgridAPIKey := os.Getenv("SENDGRID_API_KEY")

    // Conectando-se ao RabbitMQ
    conn, err := amqp.Dial(rabbitMQURL)
    if err != nil {
        log.Fatalf("Erro ao conectar ao RabbitMQ: %s", err)
    }
    defer conn.Close()

    ch, err := conn.Channel()
    if err != nil {
        log.Fatalf("Erro ao abrir o canal RabbitMQ: %s", err)
    }
    defer ch.Close()

    msgs, err := ch.Consume(
        queueName,
        "",
        true,
        false,
        false,
        false,
        nil,
    )
    if err != nil {
        log.Fatalf("Erro ao registrar consumidor RabbitMQ: %s", err)
    }

    // Loop infinito para consumir mensagens
    for msg := range msgs {
        var message Message
        if err := json.Unmarshal(msg.Body, &message); err != nil {
            log.Printf("Erro ao decodificar a mensagem: %s", err)
            continue
        }

        // Enviar e-mail via SendGrid
        if err := sendEmail(sendgridAPIKey, message.Email, message.Subject, message.Body); err != nil {
            log.Printf("Erro ao enviar e-mail: %s", err)
            continue
        }

        log.Printf("E-mail enviado para %s com sucesso!", message.Email)
    }
}

// Função para enviar e-mails via SendGrid
func sendEmail(apiKey, to, subject, body string) error {
    from := mail.NewEmail("guilhermelinosp", "guilhermelinosp@gmail.com")
    toEmail := mail.NewEmail("Recipient", to)
    plainTextContent := body
    message := mail.NewSingleEmail(from, subject, toEmail, plainTextContent, plainTextContent)
    client := sendgrid.NewSendClient(apiKey)
    _, err := client.Send(message)
    return err
}
