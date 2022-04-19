from enums import winning_pattern, nums_to_indexes_dictionary, tick_X, tick_O


board_status = [[' ', ' ', ' '],
                [' ', ' ', ' '],
                [' ', ' ', ' ']
                ]


def clean_up_board():
    global board_status
    board_status = [
        [' ', ' ', ' '],
        [' ', ' ', ' '],
        [' ', ' ', ' ']
    ]


def calculate_win(lab):
    global board_status
    pattern_list = []

    for index_of_board in range(1, 10):
        index_calculated = nums_to_indexes_dictionary[index_of_board]
        if board_status[index_calculated[0]][index_calculated[1]] == lab:
            pattern_list.append(index_of_board)
        else:
            continue

    final_set = set(pattern_list)

    for i in winning_pattern:
        lst = list(final_set & set(i))
        lst.sort()
        if lst == i:
            return True
        else:
            continue
    else:
        return False


def recreate_board(saved_data):
    global board_status
    index_tick_data = [(int(instance.index_pos), instance.tick_type) for instance in saved_data if int(instance.index_pos) != 0]

    for index, tick in index_tick_data:
        index_list_user = nums_to_indexes_dictionary[index]
        board_status[index_list_user[0]][index_list_user[1]] = tick

    return board_status


def play(saved_data):
    global board_status
    board_status = recreate_board(saved_data)

    win_x = calculate_win(tick_X)
    win_o = calculate_win(tick_O)

    if len([(int(instance.index_pos), instance.tick_type) for instance in saved_data if int(instance.index_pos) != 0]) >= 9:
        if (not win_o) and (not win_x):
            clean_up_board()
            return "It's a Tie!, try again folks", True

    if win_x:
        clean_up_board()
        return (f"Player '{[instance.p_name for instance in saved_data if instance.tick_type == tick_X][0]}' Won!"), True
    
    if win_o:
        clean_up_board()
        return (f"Player '{[instance.p_name for instance in saved_data if instance.tick_type == tick_O][0]}' Won!"), True

    return (0, False)