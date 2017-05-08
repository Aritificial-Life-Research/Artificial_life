import pika
import sys

connection = pika.BlockingConnection(pika.ConnectionParameters(
        host='localhost'))
channel = connection.channel()

# channel.exchange_declare(exchange='cube',
#                          type='topic')

routing_key = sys.argv[1] if len(sys.argv) > 2 else 'NetBrain.Input.2'
message = ' '.join(sys.argv[2:]) or '{"posX":10,"posY":1,"posZ":1}'
channel.basic_publish(exchange='NetBrain',
                      routing_key=routing_key,
                      body=message)
print(" [x] Sent %r:%r" % (routing_key, message))
connection.close()