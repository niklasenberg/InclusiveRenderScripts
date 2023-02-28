import random
import onnx
import onnxruntime
import torch
import pandas as pd
from sklearn.preprocessing import OneHotEncoder

# establish ohe
data = pd.read_csv("asd-data.csv", header=None)
x = data.iloc[:, 0:4]  # Feature set
y = data.iloc[:, 4:]  # Label set
ohe = OneHotEncoder(handle_unknown='ignore', sparse_output=False).fit(y)

# verify the ONNX model
onnx_model = onnx.load("model.onnx")
onnx.checker.check_model(onnx_model)

# create an inference session with the ONNX model
sess = onnxruntime.InferenceSession("model.onnx")

# get the input tensor name for model
all_input_names = [node.name for node in onnx_model.graph.input]
name = all_input_names[0]

n = 1000  # number of iterations
correct_predictions = 0

for i in range(n):
    # establish input tensors for each case
    # pictogram
    input_data1 = torch.tensor(
        [[random.uniform(0, 5), random.uniform(0, 5), random.uniform(0, 2), random.uniform(3, 5)]],
        dtype=torch.float32)
    # pictogramandtext
    input_data2 = torch.tensor(
        [[random.uniform(0, 5), random.uniform(0, 1), random.uniform(3, 5), random.uniform(4, 5)]],
        dtype=torch.float32)
    # static
    input_data3 = torch.tensor(
        [[random.uniform(4, 5), random.uniform(0, 2), random.uniform(0, 5), random.uniform(0, 5)]],
        dtype=torch.float32)
    # text
    input_data4 = torch.tensor(
        [[random.uniform(0, 5), random.uniform(0, 2), random.uniform(3, 5), random.uniform(3, 5)]],
        dtype=torch.float32)

    # run all inputs through model
    output1 = sess.run(None, {name: input_data1.numpy()})
    output2 = sess.run(None, {name: input_data2.numpy()})
    output3 = sess.run(None, {name: input_data3.numpy()})
    output4 = sess.run(None, {name: input_data4.numpy()})

    # -1 -4 -2 0
    # get the predicted index of each output
    predicted_index1 = torch.argmax(torch.tensor(output1[0])).item()
    predicted_index2 = torch.argmax(torch.tensor(output2[0])).item()
    predicted_index3 = torch.argmax(torch.tensor(output3[0])).item()
    predicted_index4 = torch.argmax(torch.tensor(output4[0])).item()

    # assert correct predicted label for each case
    predicted_label1 = ohe.categories_[0][predicted_index1]
    if predicted_label1 == ohe.categories_[0][0]:
        correct_predictions += 1

    predicted_label2 = ohe.categories_[0][predicted_index2]
    if predicted_label2 == ohe.categories_[0][1]:
        correct_predictions += 1

    predicted_label3 = ohe.categories_[0][predicted_index3]
    if predicted_label3 == ohe.categories_[0][2]:
        correct_predictions += 1

    predicted_label4 = ohe.categories_[0][predicted_index4]
    if predicted_label4 == ohe.categories_[0][3]:
        correct_predictions += 1

print("Accuracy:", correct_predictions / (n * 4))

for i in range(10):
    # random characteristic approximation
    # set up random input tensor
    input_data4 = torch.tensor(
        [[random.uniform(0, 5), random.uniform(0, 5), random.uniform(0, 5), random.uniform(0, 5)]],
        dtype=torch.float32)

    print("\nRandom Characteristic")
    print("Feedback stimulation: ", input_data4[0][0].item())
    print("Sensory sensitivity: ", input_data4[0][1].item())
    print("Reading comprehension: ", input_data4[0][2].item())
    print("Structure need: ", input_data4[0][3].item())

    # run prediction and print approxed label
    output4 = sess.run(None, {name: input_data4.numpy()})
    predicted_index4 = torch.argmax(torch.tensor(output4[0])).item()

    print("Label:", ohe.categories_[0][predicted_index4])
