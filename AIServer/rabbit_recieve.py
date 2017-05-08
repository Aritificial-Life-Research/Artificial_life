#!/usr/bin/env python
import pika
import sys

connection = pika.BlockingConnection(pika.ConnectionParameters(host='localhost'))
channel = connection.channel()

channel.exchange_declare(exchange='NetBrain',
                         type='topic')

Q1 = channel.queue_declare(exclusive=True)
Q2 = channel.queue_declare(exclusive=True)
q1_name = Q1.method.queue


channel.queue_bind(exchange='NetBrain',
                   queue=q1_name,
                   routing_key="Output.1")

channel.queue_bind(exchange='NetBrain',
                   queue=q1_name,
                   routing_key="Output.2")


def callback(ch, method, properties, body):
    print(" [x] %r:%r" % (method.routing_key, body))

channel.basic_consume(callback,
                      queue=q1_name,
                      no_ack=True)

channel.start_consuming()